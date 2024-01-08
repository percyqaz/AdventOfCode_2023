type Pulse = Low | High

type PulseEvent = { Kind: Pulse; From: string; To: string }

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

    override this.Targets = targets
    override this.Init input = memory <- memory.Add (input, Low)
    override this.Pulse(kind: Pulse, from: string) =
        memory <- memory.Add (from, kind)
        if Map.forall (fun _ value -> value = High) memory then Some Low else Some High

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

let pulse(kind: Pulse) : uint64 * uint64 =
    let mutable low = 0uL
    let mutable high = 0uL
    if kind = Low then low <- low + 1uL else high <- high + 1uL

    for m in BROADCAST_ROOT do
        pulse_queue.Add { Kind = kind; From = "broadcaster"; To = m }

    while pulse_queue.Count > 0 do
        let top = pulse_queue.[0]
        if top.Kind = Low then low <- low + 1uL else high <- high + 1uL
        pulse_queue.RemoveAt(0)

        if modules.ContainsKey top.To then
            match modules.[top.To].Pulse(top.Kind, top.From) with
            | Some kind ->
                for target in modules.[top.To].Targets do
                    pulse_queue.Add { Kind = kind; From = top.To; To = target }
            | None -> ()

    low, high

let mutable low = 0uL
let mutable high = 0uL
for i = 0 to 999 do
    let l, h = pulse Low
    low <- low + l
    high <- high + h
printfn "Hi: %i; Lo: %i; Product: %i" high low (high * low)