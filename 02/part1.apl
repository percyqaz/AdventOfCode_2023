split←{(~⍵∊⍺)⊂,⍵}
lines←⊃1↓¨':'split¨⎕FIO[49] 'input.txt'
games←1↓¨¨¨','split¨¨';'split¨lines
int←{10⊥(¯48)+¨⎕UCS⍵}
possible←{(↑⍵)≤⌈/(12×'r'=↑1↓⍵),(13×'g'=↑1↓⍵),(14×'b'=↑1↓⍵)}
ones←,⌊/¨⌊/¨¨possible¨¨¨{(int↑⍵),↑↑1↓⍵}¨¨¨' 'split¨¨¨games
+/ones/⍳↑⍴lines