open System.IO

let map = File.ReadAllLines("input.txt") |> Array.map (fun s -> s.Trim() |> Seq.toArray)
let map_width = map.[0].Length
let start = (1, 0)

type Path =
    {
        Head: (int * int)
        History: (int * int) Set
    }
    member this.NextSteps =
        seq {
            let x, y = this.Head
            if x < map_width - 1 && map.[y].[x + 1] <> '#' then
                yield (x + 1, y)
            if x > 0 && map.[y].[x - 1] <> '#' then
                yield (x - 1, y)
            if y < map.Length - 1 && map.[y + 1].[x] <> '#' then
                yield (x, y + 1)
            if y > 0 && map.[y - 1].[x] <> '#' then
                yield (x, y - 1)
        }
        |> Seq.filter (fun next -> Set.contains next this.History |> not)
    member this.Step (next: int * int) =
        {
            Head = next
            History = Set.add this.Head this.History
        }
    member this.Length = this.History.Count

let mutable paths =
    [
        {
            Head = start
            History = Set.empty
        }
    ] |> System.Collections.Generic.Stack

let pop_path () =
    match paths.TryPop() with
    | true, result -> Some result
    | false, _ -> None

let add_path (p: Path) = 
    paths.Push p

let mutable forks = Set.empty

let mutable best = 0

let solve () =
    let mutable next = pop_path()
    while next.IsSome do
        let path = next.Value
        let mutable dead_path = true
        for next in path.NextSteps do
            if not dead_path && not (forks.Contains path.Head) then
                printfn "fork at %A" path.Head
                forks <- forks.Add path.Head
            dead_path <- false
            add_path (path.Step next)
        if dead_path && snd path.Head + 1 = map.Length then
            let l = path.Length
            if l > best then
                printfn "path found length %i" path.Length
                best <- l
        next <- pop_path()

solve ()