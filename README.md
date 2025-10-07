## Cslox Grammar

| Name        | Operators         | Associates  |
| ----------- | ----------------- | ----------- |
| Equality    | `==` `!=`         | Left        |
| Comparison  | `>` `>=` `<` `<=` | Left        |
| Term        | `-` `+`           | Left        |
| Factor      | `/` `*`           | Left        |
| Unary       | `!` `-`           | Right       |

```text
expression -> literal
            | unary
            | binary
            | grouping ;
literal    -> NUMBER | STRING | "true" | "false" | "nil" ;
grouping   -> "(" expression ")" ;
unary      -> ( "-" | "!" ) expression ;
binary     -> expression operator expression ;
operator   -> "==" | "!=" | "<" | "<=" | ">" | ">="
            | "+" | "-" | "*" | "/" ;
```

```text
expression -> comma ;
comma      -> ternary ( "," ternary )* ;
ternary    -> equality ( "?" equality ":" ternary )? ;
equality   -> comparison ( ( "!=" | "==" ) comparison )* ;
comparison -> term ( ( ">" | ">=" | "<" | "<=" ) term )* ;
term       -> factor ( ( "-" | "+" ) factor )* ;
factor     -> unary ( ( "/" | "*" ) unary )* ;
unary      -> ( "!" | "-" ) unary
            | primary;
primary    -> NUMBER
            | STRING
            | "true"
            | "false"
            | "nil"
            | "(" expression ")" ;
