differences :: [Integer] -> [Integer]
differences (x : y : xs) = (y - x) : differences (y : xs)
differences _ = []

zero 0 = True
zero _ = False

predict_next :: [Integer] -> Integer
predict_next xs = 
    if all zero xs then 0
    else last xs + predict_next (differences xs)

main :: IO ()
main = do 
    txt <- readFile $ "input.txt"
    let sequences = map (map read . words) . lines $ txt :: [[Integer]]
    let predictions = map predict_next $ sequences
    let sum = foldl (+) 0 predictions
    print $ sum