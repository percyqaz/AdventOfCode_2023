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
 
 type Part =
    {
        ExtremelyCoolLooking: int
        Musical: int
        Aerodynamic: int
        Shiny: int
    }
    static member Parse(s: string) =
        let split = s.Split([|'='; ','; '}'|])
        {
            ExtremelyCoolLooking = int split.[1]
            Musical = int split.[3]
            Aerodynamic = int split.[5]
            Shiny = int split.[7]
        }
    member this.Attribute(attribute: Attribute) =
        match attribute with
        | ExtremelyCoolLooking -> this.ExtremelyCoolLooking
        | Musical -> this.Musical
        | Aerodynamic -> this.Aerodynamic
        | Shiny -> this.Shiny
 
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

open System.IO
open System.Collections.Generic

let lines = File.ReadAllLines("input.txt");;
let rules = new Dictionary<string, Rule>();;
let mutable parsing_rules = true;;

let rec eval (rule: Rule) (part: Part) : bool =
    let rec clause_loop (clauses: RuleClause list) =
        match clauses with
        | [] -> None
        | GreaterThan (attr, amount, result) :: xs ->
            if part.Attribute attr > amount then
                match result with
                | Accept -> Some true
                | Reject -> Some false
                | AnotherRule r -> Some (eval rules.[r] part)
            else clause_loop xs
        | LessThan (attr, amount, result) :: xs ->
            if part.Attribute attr < amount then
                match result with
                | Accept -> Some true
                | Reject -> Some false
                | AnotherRule r -> Some (eval rules.[r] part)
            else clause_loop xs

    match clause_loop rule.Clauses with
    | Some clause_hit -> clause_hit
    | None -> 
        match rule.Else with
        | Accept -> true
        | Reject -> false
        | AnotherRule r -> eval rules.[r] part

let mutable accepted_value = 0;;

for line in lines do
    if line = "" then
        parsing_rules <- false
    elif parsing_rules then
        let name_length = line.IndexOf("{")
        rules[line.Substring(0, name_length)] <- Rule.Parse (line.Substring name_length)
    else
        let part = Part.Parse line
        if eval rules.["in"] part then 
            accepted_value <- accepted_value + part.ExtremelyCoolLooking + part.Musical + part.Aerodynamic + part.Shiny
printfn "%i acceptance score" accepted_value