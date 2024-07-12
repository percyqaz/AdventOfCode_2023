type Axis =
    {
        Min: int
        Max: int
    }
    member this.Overlaps(other: Axis) =
        other.Min <= this.Max && other.Max >= this.Min
    member this.Contains(value: int) =
        value >= this.Min && value <= this.Max
    member this.Translate(value: int) =
        { Min = this.Min + value; Max = this.Max + value }

type Box =
    {
        X: Axis
        Y: Axis
        mutable Z: Axis
    }
    member this.Overlaps(other: Box) =
        this.X.Overlaps other.X && this.Y.Overlaps other.Y && this.Z.Overlaps other.Z
    member this.Contains(x, y, z) =
        this.X.Contains y && this.Y.Contains y && this.Z.Contains z
    member this.MoveDown() : Box =
        this.Z <- this.Z.Translate -1
        this
    member this.MoveUp() =
        this.Z <- this.Z.Translate 1

// negative z = gravity

open System.IO

let lines = File.ReadAllLines("input.txt");;

let boxes =
    seq {
        for line in lines do
            let numbers = line.Trim().Split("~") |> Array.map (fun s -> s.Split(",")) |> Array.concat |> Array.map int
            assert(numbers.Length = 6)
            yield {
                X = { Min = numbers.[0]; Max = numbers.[3] }
                Y = { Min = numbers.[1]; Max = numbers.[4] }
                Z = { Min = numbers.[2]; Max = numbers.[5] }
            }
    }
    |> Seq.sortBy (fun box -> box.Z.Min)
    |> Array.ofSeq

let mutable supported_by : Map<int, int Set> = Map.empty
let mutable supports : Map<int, int Set> = Map.empty
let add_support (support: int) (target: int) =
    supported_by <- 
        supported_by.Change (
            target, 
            function
            | None -> Set.singleton support |> Some
            | Some set -> Set.add support set |> Some
        )
    supports <- 
        supports.Change (
            support, 
            function
            | None -> Set.singleton target |> Some
            | Some set -> Set.add target set |> Some
        )


for i = 0 to boxes.Length - 1 do
    let box = boxes.[i]
    let mutable done_with_box = false
    while not done_with_box && box.Z.Min > 1 do
        let down = box.MoveDown()
        for j = 0 to i - 1 do
            if down.Overlaps boxes.[j] then
                add_support j i
                done_with_box <- true
        if done_with_box then
            box.MoveUp()

let can_be_disintegrated (id: int) =
    let mutable it_can = true
    for supported in Map.tryFind id supports |> Option.defaultValue Set.empty do
        let supported_supported_by = Map.tryFind supported supported_by |> Option.defaultValue Set.empty
        if supported_supported_by.Count = 1 then
            it_can <- false
    it_can

{ 0 .. boxes.Length - 1} |> Seq.where can_be_disintegrated |> Seq.length |> printfn "%i"