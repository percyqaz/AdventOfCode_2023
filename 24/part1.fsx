open System

type Vec3 =
    {
        X: float
        Y: float
        Z: float
    }
    member this.Add(other: Vec3) =
        { 
            X = this.X + other.X
            Y = this.Y + other.Y
            Z = this.Z + other.Z
        }
    member this.Normalise =
        {
            X = 1.0
            Y = Math.Round(this.Y / this.X, 2)
            Z = Math.Round(this.Z / this.X, 2)
        }

type Intersection =
    | Never
    | Future of float * float
    | SomethingElse

type Hailstone =
    {
        Position: Vec3
        Velocity: Vec3
    }
    member this.IntersectXY(other: Hailstone) =

        if this.Velocity.Normalise = other.Velocity.Normalise then Never else

        let m1 = this.Velocity.Y / this.Velocity.X
        let m2 = other.Velocity.Y / other.Velocity.X

        let c1 = this.Position.Y - m1 * this.Position.X
        let c2= other.Position.Y - m2 * other.Position.X

        let x = (c2 - c1) / (m1 - m2)
        let y = m1 * x + c1

        if sign (x - this.Position.X) = sign this.Velocity.X && sign (x - other.Position.X) = sign other.Velocity.X then
            Future (x, y)
        else SomethingElse

open System.IO

let lines = File.ReadAllLines("input.txt")
let hailstones =
    seq {
        for line in lines do
            let numbers = line.Trim().Replace(" @ ", ",").Replace(" ", "").Split(",") |> Array.map float
            yield {
                Position = { X = numbers.[0]; Y = numbers.[1]; Z = numbers.[2] }
                Velocity = { X = numbers.[3]; Y = numbers.[4]; Z = numbers.[5] }
            }
    }
    |> Array.ofSeq

let TEST_MIN = 200000000000000.0
let TEST_MAX = 400000000000000.0

let mutable count = 0
for i = 0 to hailstones.Length - 1 do
    for j = i + 1 to hailstones.Length - 1 do
        match hailstones.[i].IntersectXY hailstones.[j] with
        | Future (x, y) when x >= TEST_MIN && y >= TEST_MIN && x <= TEST_MAX && y <= TEST_MAX ->
            count <- count + 1
        | _ -> ()
printfn "future crossings within test area: %i" count