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
            let current_tile = map.[y].[x]
            match current_tile with
            | '>' -> yield (x + 1, y)
            | '<' -> yield (x - 1, y)
            | '^' -> yield (x, y - 1)
            | 'v' -> yield (x, y + 1)
            | _ ->
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
    ]

while paths <> [] do
    paths <-
        paths 
        |> Seq.collect (fun path -> 
            seq {
                let mutable dead_path = true
                for next in path.NextSteps do
                    dead_path <- false
                    yield path.Step next
                if dead_path && snd path.Head + 1 = map.Length then
                    printfn "path found length %i" path.Length
            }
        )
        |> List.ofSeq
        