is_digit('0').
is_digit('1').
is_digit('2').
is_digit('3').
is_digit('4').
is_digit('5').
is_digit('6').
is_digit('7').
is_digit('8').
is_digit('9').

readFile(Lines):-
    open('input.txt', read, File),
    read_lines(File, Lines),
    close(File).

read_lines(File,[]):- 
    at_end_of_stream(File).

read_lines(File,[X|L]):-
    \+ at_end_of_stream(File),
    get_char(File,X),
    read_lines(File,L).
  
indexesOf([Element|_], Element, 0).
indexesOf([_|Tail], Element, Index):-
  indexesOf(Tail, Element, Index1),
  Index is Index1+1.
  
take_three(Xs,N) :- take_three(Xs,0,N).
take_three([A,B,C|_],N,N) :- print(A), print(B), print(C).
take_three([_|Xs],V,N) :-
	V1 is V+1,
	take_three(Xs,V1,N).
  
digit_at(Xs,N) :- digit_at(Xs,0,N).
digit_at([X|_],N,N) :- is_digit(X).
digit_at([_|Xs],V,N) :-
	V1 is V+1,
	digit_at(Xs,V1,N).
	
positionXY(Xs,Width,X,Y,C) :- 
	xyToN(X,Y,Width,N),
	position(Xs,N,C).
	
nToXY(X,Y,Width,N) :-
    Y is N // (Width + 1),
    X is N - Y * (Width + 1).

xyToN(X,Y,Width,N) :-
	N is X + Y * (Width + 1).

consecutive(A,B) :- B is A - 1.
consecutive(A,B) :- B is A + 1.

neighbor(X,Y,X2,Y2) :- 
	consecutive(X,X2),
	consecutive(Y,Y2).
neighbor(X,Y,X2,Y2) :- 
	consecutive(X,X2),
	Y = Y2.
neighbor(X,Y,X2,Y2) :- 
	X = X2,
	consecutive(Y,Y2).
	
leftmost_digit(_,0).
leftmost_digit(Lines,NStart,N) :-
	N2 is NStart - 1,
	digit_at(Lines,N2),
	leftmost_digit(Lines,N2,N).
leftmost_digit(Lines,NStart,N) :-
	N2 is NStart - 1,
	\+ digit_at(Lines,N2),
	N is NStart.
	
mymember(X,[X|_]).
mymember(X,[_|T]) :- mymember(X,T).

not(A) :- \+ call(A).

set([],[]).
set([H|T],[H|Out]) :-
    not(mymember(H,T)),
    set(T,Out).
set([H|T],Out) :-
    mymember(H,T),
    set(T,Out).
	
take_threes(_,[]).
take_threes(Lines,[X|Xs]) :- take_three(Lines,X), print('*'), take_threes(Lines,Xs).
	
main :- 
	readFile(Lines),
	indexesOf(Lines,'\n',Width),
	!,
	indexesOf(Lines,'*',N),
	findall(NOut, (nToXY(X,Y,Width,N), neighbor(X,Y,X2,Y2), xyToN(X2,Y2,Width,N2), digit_at(Lines,N2), leftmost_digit(Lines,N2,NOut)), Ls),
	set(Ls,Out),
	nl, print(N), print('-'), take_threes(Lines,Out).