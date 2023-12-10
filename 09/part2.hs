differences :: [Integer] -> [Integer]
differences (x : y : xs) = (y - x) : differences (y : xs)
differences _ = []

zero 0 = True
zero _ = False

predict_previous :: [Integer] -> Integer
predict_previous xs = 
    if all zero xs then 0
    else head xs - predict_previous (differences xs)

main :: IO ()
main = do 
    txt <- readFile $ "input.txt"
    let sequences = map (map read . words) . lines $ txt :: [[Integer]]
    let predictions = map predict_previous $ sequences
    let sum = foldl (+) 0 predictions
    print $ sum