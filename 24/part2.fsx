type Vec3 =
    {
        X: int64
        Y: int64
        Z: int64
    }
    member this.Add(other: Vec3) =
        { 
            X = this.X + other.X
            Y = this.Y + other.Y
            Z = this.Z + other.Z
        }

type Hailstone =
    {
        Position: Vec3
        Velocity: Vec3
    }
    member this.IntersectX(other: Hailstone) =
        let pdelta = other.Position.X - this.Position.X
        let vdelta = this.Velocity.X - other.Velocity.X
        if vdelta = 0L then
            if pdelta = 0L then
                Some -1L
            else None
        else
            if sign pdelta = sign vdelta && pdelta % vdelta = 0 then
                Some (pdelta / vdelta)
            else None
    member this.IntersectY(other: Hailstone) =
        let pdelta = other.Position.Y - this.Position.Y
        let vdelta = this.Velocity.Y - other.Velocity.Y
        if vdelta = 0L then
            if pdelta = 0L then
                Some -1L
            else None
        else
            if sign pdelta = sign vdelta && pdelta % vdelta = 0 then
                Some (pdelta / vdelta)
            else None
    member this.IntersectZ(other: Hailstone) =
        let pdelta = other.Position.Z - this.Position.Z
        let vdelta = this.Velocity.Z - other.Velocity.Z
        if vdelta = 0L then
            if pdelta = 0L then
                Some -1L
            else None
        else
            if sign pdelta = sign vdelta && pdelta % vdelta = 0 then
                Some (pdelta / vdelta)
            else None

open System.IO

let lines = File.ReadAllLines("input.txt")
let hailstones =
    seq {
        for line in lines do
            let numbers = line.Trim().Replace(" @ ", ",").Replace(" ", "").Split(",") |> Array.map int64
            yield {
                Position = { X = numbers.[0]; Y = numbers.[1]; Z = numbers.[2] }
                Velocity = { X = numbers.[3]; Y = numbers.[4]; Z = numbers.[5] }
            }
    }
    |> Array.ofSeq


let rec gcd a b =
    if a = b then a
    elif a > b then gcd (a - b) b
    else gcd a (b - a)

let lcm a b = a * b / gcd a b

let compatible (mult1, offset1) (mult2, offset2) : (int64 * int64) option =
    let gcd = gcd mult1 mult2
    let lcm = lcm mult1 mult2
    { 0L .. mult2 * gcd } 
    |> Seq.tryFind (fun i -> 
        (i * mult1 + offset1) % mult2 = offset2
    )
    |> Option.map (fun i -> lcm, (i * mult1 + offset1) % lcm)

type Solution = { Velocity: int64; Solution: int64 * int64 }

let verify_vel (position_velocities: (int64 * int64) array) (candidate_vel: int64) =
    try
        let mutable current = (1L, 0L)

        for pz, vz in position_velocities do
            let vdelta = abs(vz - candidate_vel)

            if vdelta <> 0 then
                match compatible current (vdelta, pz % vdelta) with
                | Some next -> 
                    current <- next
                | None -> failwith "not possible"

        Some { Velocity = candidate_vel; Solution = current }
    with _ -> None

type Solution2 =
    {
        Velocity: int64
        Solution: int64 * int64
        Start: int64
    }

let verify_solution (position_velocities: (int64 * int64) array) { Velocity = velocity; Solution = (multiple, offset) } =
    let accept(current) =
        position_velocities 
        |> Array.forall 
            (fun (pz, vz) -> 
                if vz = velocity then
                    pz = current
                else (pz - current) / (velocity - vz) > 0L
        )
        
    seq { for i = 0L to 100L do yield multiple * i + offset; yield multiple * -i + offset } 
    |> Seq.tryFind accept
    |> Option.map (fun start -> { Start = start; Velocity = velocity; Solution = (multiple, offset) })

let xs = hailstones |> Array.map (fun h -> h.Position.X, h.Velocity.X)
let ys = hailstones |> Array.map (fun h -> h.Position.Y, h.Velocity.Y)
let zs = hailstones |> Array.map (fun h -> h.Position.Z, h.Velocity.Z)

let numbers = seq { for i = 1L to 1000L do yield i; yield -i }
let x_sols = 
    numbers 
    |> Seq.choose (verify_vel xs) 
    |> Seq.map (fun s -> printfn "X vel solution: %A" s; s) 
    |> Seq.choose (verify_solution xs)
    |> Seq.map (fun s -> printfn "X solution: %A" s; s) 
    |> Array.ofSeq
let y_sols = 
    numbers
    |> Seq.choose (verify_vel ys)
    |> Seq.map (fun s -> printfn "Y vel solution: %A" s; s) 
    |> Seq.choose (verify_solution ys)
    |> Seq.map (fun s -> printfn "Y solution: %A" s; s) 
    |> Array.ofSeq
let z_sols = 
    numbers 
    |> Seq.choose (verify_vel zs)
    |> Seq.map (fun s -> printfn "Z vel solution: %A" s; s) 
    |> Seq.choose (verify_solution zs)
    |> Seq.map (fun s -> printfn "Z solution: %A" s; s) 
    |> Array.ofSeq

for x in x_sols do
    for y in y_sols do
        for z in z_sols do
            let hailstone =
                {
                    Position = { X = x.Start; Y = y.Start; Z = z.Start }
                    Velocity = { X = x.Velocity; Y = y.Velocity; Z = z.Velocity }
                }
            let ok = 
                Seq.forall (fun h -> 
                    let xi = hailstone.IntersectX(h)
                    let yi = hailstone.IntersectY(h)
                    let zi = hailstone.IntersectZ(h)
                    match xi, yi, zi with
                    | Some a, Some b, Some c when a = b && b = c -> true
                    | Some a, Some b, Some -1L when a = b -> true
                    | Some a, Some -1L, Some c when a = c -> true
                    | Some -1L, Some b, Some c when b = c -> true
                    | Some -1L, Some _, Some -1L
                    | Some _, Some -1L, Some -1L
                    | Some -1L, Some -1L, Some _ -> true
                    | _ -> false
                ) hailstones
            if ok then
                printfn "3d solution: X %A Y %A Z %A" x y z
                printfn "character of %i" (x.Start + y.Start + z.Start)