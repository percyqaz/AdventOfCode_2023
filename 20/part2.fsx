[<Struct>]
type Pulse = Low | High

[<Struct>]
type PulseEvent = { Kind: Pulse; From: string; To: string }

let mutable presses = 0

[<AbstractClass>]
type Module() =

    abstract member Targets : string array
    abstract member Init : string -> unit
    abstract member Pulse : Pulse * string -> Pulse option

type FlipFlop(targets: string array) =
    inherit Module()

    let mutable state = Low

    override this.Targets = targets
    override this.Init _ = ()
    override this.Pulse(kind: Pulse, _: string) =
        match kind with
        | Low ->
            state <- if state = Low then High else Low
            Some state
        | High -> None

type Conjunction(targets: string array) =
    inherit Module()

    let mutable memory = Map.empty
    let mutable snapshots = Map.empty

    override this.Targets = targets
    override this.Init input = memory <- memory.Add (input, Low)
    override this.Pulse(kind: Pulse, from: string) =
        memory <- memory.Add (from, kind)
        if Map.forall (fun _ value -> value = High) memory then
            let snapshot_key = sprintf "%A" memory
            if not (snapshots.ContainsKey snapshot_key) then
                printfn "#%i: %s" presses snapshot_key
                snapshots <- snapshots.Add (snapshot_key, presses)
            Some Low 
        else Some High

open System.IO
open System.Collections.Generic

let lines = File.ReadAllLines("input.txt");;
let modules = new Dictionary<string, Module>();;

let BROADCAST_ROOT = lines.[0].Split(" -> ").[1].Split(", ")
for i = 1 to lines.Length - 1 do
    let split = lines.[i].Split(" -> ")
    if split.[0].[0] = '%' then
        modules.Add(split.[0].Substring(1), FlipFlop(split.[1].Split(", ")))
    else
        modules.Add(split.[0].Substring(1), Conjunction(split.[1].Split(", ")))

for m in BROADCAST_ROOT do
    modules.[m].Init "broadcaster"
for m in modules.Keys do
    for n in modules.[m].Targets do
        if modules.ContainsKey n then
            modules.[n].Init m

let mutable pulse_queue: ResizeArray<PulseEvent> = ResizeArray()

let pulse(kind: Pulse) : bool =
    let mutable rx_pulsed = false

    for m in BROADCAST_ROOT do
        pulse_queue.Add { Kind = kind; From = "broadcaster"; To = m }

    while pulse_queue.Count > 0 && not rx_pulsed do
        let top = pulse_queue.[0]
        if top.Kind = Low && top.To = "rx" then rx_pulsed <- true
        pulse_queue.RemoveAt(0)

        if modules.ContainsKey top.To then
            match modules.[top.To].Pulse(top.Kind, top.From) with
            | Some kind ->
                for target in modules.[top.To].Targets do
                    pulse_queue.Add { Kind = kind; From = top.To; To = target }
            | None -> ()

    rx_pulsed

let mutable rx_pulsed = false
while not rx_pulsed do
    presses <- presses + 1
    if pulse Low then
        rx_pulsed <- true
        printfn "rx received LOW pulse after button press #%i" presses
    elif presses % 100000 = 0 then printfn "%i" presses