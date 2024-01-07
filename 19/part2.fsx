 type Attribute =
    | ExtremelyCoolLooking
    | Musical
    | Aerodynamic
    | Shiny
    static member Parse(s: string) =
        match s with
        | "x" -> ExtremelyCoolLooking
        | "m" -> Musical
        | "a" -> Aerodynamic
        | "s" -> Shiny
        | _ -> failwithf "Unexpected attribute '%s'" s
 
 type RuleResult =
    | Accept
    | Reject
    | AnotherRule of string
    static member Parse(s: string) =
        match s with
        | "A" -> Accept
        | "R" -> Reject
        | s -> AnotherRule s

type RuleClause =
    | GreaterThan of Attribute * int * RuleResult
    | LessThan of Attribute * int * RuleResult
    static member Parse(s: string) =
        let split = s.Split(':')
        if split[0].Contains('<') then
            let split_2 = split[0].Split('<')
            LessThan (Attribute.Parse split_2[0], int split_2[1], RuleResult.Parse split[1])
        else
            let split_2 = split[0].Split('>')
            GreaterThan (Attribute.Parse split_2[0], int split_2[1], RuleResult.Parse split[1])

type Rule =
    {
        Clauses: RuleClause list
        Else: RuleResult
    }
    static member Parse(s: string) =
        let split = s.Substring(1, s.Length - 2).Split(',')
        {
            Clauses = split |> List.ofArray |> List.take (split.Length - 1) |> List.map RuleClause.Parse
            Else = RuleResult.Parse split.[split.Length - 1]
        }

type Restriction =
    {
        MinX: int
        MaxX: int
        MinM: int
        MaxM: int
        MinA: int
        MaxA: int
        MinS: int
        MaxS: int
    }

    static member Default =
        {
            MinX = 1
            MaxX = 4000
            MinM = 1
            MaxM = 4000
            MinA = 1
            MaxA = 4000
            MinS = 1
            MaxS = 4000
        }

    member this.Count = 
        uint64 (this.MaxX - this.MinX + 1)
        * uint64 (this.MaxM - this.MinM + 1)
        * uint64 (this.MaxA - this.MinA + 1)
        * uint64 (this.MaxS - this.MinS + 1)

    member this.Empty = 
        this.MaxX < this.MinX
        || this.MaxM < this.MinM
        || this.MaxA < this.MinA
        || this.MaxS < this.MinS

    member this.Intersect other =
        {
            MinX = max this.MinX other.MinX
            MaxX = min this.MaxX other.MaxX
            MinM = max this.MinM other.MinM
            MaxM = min this.MaxM other.MaxM
            MinA = max this.MinA other.MinA
            MaxA = min this.MaxA other.MaxA
            MinS = max this.MinS other.MinS
            MaxS = min this.MaxS other.MaxS
        }

open System.IO
open System.Collections.Generic

let lines = File.ReadAllLines("input.txt");;
let rules = new Dictionary<string, Rule>();;
let mutable parsing_rules = true;;

let rec restrictions (rule: Rule) (res: Restriction) : Restriction seq =

    let rec clause_loop (clauses: RuleClause list) (res: Restriction) : Restriction seq =
        match clauses with
        | [] -> 
            seq {
                if not res.Empty then
                    match rule.Else with
                    | Accept -> yield res
                    | Reject -> ()
                    | AnotherRule r -> yield! restrictions rules.[r] res
            }
        | GreaterThan (attr, amount, result) :: xs ->
            let res_if_met =
                match attr with
                | ExtremelyCoolLooking -> { res with MinX = max res.MinX (amount + 1) }
                | Musical -> { res with MinM = max res.MinM (amount + 1) }
                | Aerodynamic ->{ res with MinA = max res.MinA (amount + 1) }
                | Shiny -> { res with MinS = max res.MinS (amount + 1) }
            let res_if_not_met = 
                match attr with
                | ExtremelyCoolLooking -> { res with MaxX = min res.MaxX amount }
                | Musical -> { res with MaxM = min res.MaxM amount }
                | Aerodynamic -> { res with MaxA = min res.MaxA amount }
                | Shiny -> { res with MaxS = min res.MaxS amount }

            seq {
                if not res_if_met.Empty then
                    match result with
                    | Accept -> yield res_if_met
                    | Reject -> ()
                    | AnotherRule r -> yield! restrictions rules.[r] res_if_met

                yield! clause_loop xs res_if_not_met
            }

        | LessThan (attr, amount, result) :: xs ->
            let res_if_met =
                match attr with
                | ExtremelyCoolLooking -> { res with MaxX = min res.MaxX (amount - 1) }
                | Musical -> { res with MaxM = min res.MaxM (amount - 1) }
                | Aerodynamic -> { res with MaxA = min res.MaxA (amount - 1) }
                | Shiny -> { res with MaxS = min res.MaxS (amount - 1) }
            let res_if_not_met = 
                match attr with
                | ExtremelyCoolLooking -> { res with MinX = max res.MinX amount }
                | Musical -> { res with MinM = max res.MinM amount }
                | Aerodynamic ->{ res with MinA = max res.MinA amount }
                | Shiny -> { res with MinS = max res.MinS amount }

            seq {
                if not res_if_met.Empty then
                    match result with
                    | Accept -> yield res_if_met
                    | Reject -> ()
                    | AnotherRule r -> yield! restrictions rules.[r] res_if_met

                yield! clause_loop xs res_if_not_met
            }

    clause_loop rule.Clauses res

let mutable count = 0uL

let rec counter(xs: Restriction list) =
    match xs with
    | x :: xs -> 
        count <- count + x.Count
        for r in xs do
            let intersect = x.Intersect r
            if not intersect.Empty then
                count <- count - intersect.Count
        counter xs
    | [] -> ()

for line in lines do
    if line = "" then
        parsing_rules <- false
    elif parsing_rules then
        let name_length = line.IndexOf("{")
        rules[line.Substring(0, name_length)] <- Rule.Parse (line.Substring name_length)
    
let xs = restrictions rules.["in"] Restriction.Default |> List.ofSeq
counter xs

printfn "%i" count