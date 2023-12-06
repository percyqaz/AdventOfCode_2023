Solution: Let X be the time you have and Y be the record distance

`2 * sqrt(X * X / 4 - Y)` will be within 1 of the correct answer for your guess-and-test purposes, and rounding it will give you the correct answer most of the time  
I haven't fully checked but rounding appears to give the right answer when X is odd, and is out by +-1 when X is even

Equation + guess-and-testing done while walking with some assistance from my phone calculator

How to get this equation:

Note how you want to find integers A and B, where A + B = X and A * B > Y
Wlog let B be the bigger of the 2
You want the solution with the least possible value of A (which is also when A * B = Y)

Then for example if you get 3x8 = 24, the other ways of getting 24 or higher are 4x7, 5x6, 6x5, 7x4, 8x3.
This is exactly B - A + 1 solutions if A and B are integer solutions

This means solutions are related to the value of N when (X/2 - N)(X/2 + N) = Y; There would be 2N solutions when X is even, 2N+1 when X is odd.

This rearranges to X^2/4 - N^2 = Y  
to X^2/4 - Y = N^2  
to N = sqrt(X * X / 4 - Y)  
to #solutions ~= 2 * sqrt(X * X / 4 - Y)

Use this method or intermediate parts of this method to find N, or A and B, to guess and test your part 2 solution efficiently