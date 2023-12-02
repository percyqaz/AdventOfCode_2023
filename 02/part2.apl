split←{(~⍵∊⍺)⊂,⍵}
lines←⊃1↓¨':'split¨⎕FIO[49] 'input.txt'
int←{10⊥(¯48)+¨⎕UCS⍵}
games←⊃¨,/¨,{(int↑⍵),↑↑1↓⍵}¨¨¨' 'split¨¨¨1↓¨¨¨','split¨¨';'split¨lines
red←{⌈/↑¨({'r'=↑1↓⍵}¨⍵)/⍵}
green←{⌈/↑¨({'g'=↑1↓⍵}¨⍵)/⍵}
blue←{⌈/↑¨({'b'=↑1↓⍵}¨⍵)/⍵}
+/{×/(red ⍵),(green ⍵),(blue ⍵)}¨games