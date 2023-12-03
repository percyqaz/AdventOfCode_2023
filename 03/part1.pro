is_symbol('*').
is_symbol('=').
is_symbol('/').
is_symbol('%').
is_symbol('#').
is_symbol('&').
is_symbol('$').
is_symbol('+').
is_symbol('-').
is_symbol('@').

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
	
indexOf([Element|_], Element, 0):- !.
indexOf([_|Tail], Element, Index):-
  indexOf(Tail, Element, Index1),
  !,
  Index is Index1+1.
  
indexesOf([Element|_], Element, 0).
indexesOf([_|Tail], Element, Index):-
  indexesOf(Tail, Element, Index1),
  Index is Index1+1.
  
take_three(Xs,N) :- take_three(Xs,0,N).
take_three([A,B,C|_],N,N) :- print(A), print(B), print(C).
take_three([_|Xs],V,N) :-
	V1 is V+1,
	take_three(Xs,V1,N).
  
position(Xs,N,C) :- position_inner(Xs,0,N,C).
position_inner([X|_],N,N,X).
position_inner([_|Xs],V,N,C) :-
	V1 is V+1,
	position_inner(Xs,V1,N,C).
	
positionXY(Xs,Width,X,Y,C) :- 
	xyToN(X,Y,Width,N),
	position(Xs,N,C).
	
nToXY(X,Y,Width,N) :-
    Y is N // (Width + 1),
    X is N - Y * (Width + 1).

xyToN(X,Y,Width,N) :-
	N is X + Y * (Width + 1).

consecutive(A,B) :- B is A + 1.
consecutive(A,B) :- B is A - 1.

neighbor(X,Y,X2,Y2) :- 
	consecutive(Y,Y2),
	!,
	consecutive(X,X2).
neighbor(X,Y,X2,Y2) :- 
	consecutive(X,X2),
	Y = Y2.
neighbor(X,Y,X2,Y2) :- 
	consecutive(Y,Y2),
	!,
	X = X2.

partofnumber(Lines,N) :- 
	is_digit(D),
	position(Lines,N,D),
	N1 is N + 2,
	partofnumber_n(Lines,N1).
partofnumber(Lines,N) :- 
	is_digit(D),
	position(Lines,N,D),
	N1 is N + 1,
	partofnumber_n(Lines,N1).
partofnumber(Lines,N) :- partofnumber_n(Lines,N).
partofnumber_n(Lines,N) :-
	indexOf(Lines,'\n',Width),
	!,
	is_digit(D),
	position(Lines,N,D),
	is_symbol(S),
	nToXY(X,Y,Width,N),
	neighbor(X,Y,X2,Y2),
	xyToN(X2,Y2,Width,N2),
	position(Lines,N2,S).
	
startofnumber(Lines,N) :-
	indexOf(Lines,'\n',Width),
	!,
	partofnumber(Lines,N),
	nToXY(X,Y,Width,N),
	X2 is X - 1,
	xyToN(X2,Y,Width,N2),
	position(Lines,N2,C),
	\+ is_digit(C).
startofnumber(Lines,N) :-
	partofnumber(Lines,N),
	N = 0.
	
main :- 
	readFile(Lines),
	!,
	startofnumber(Lines,N),
	nl, print(N), print('-'), take_three(Lines,N).