LOAD CSV WITH HEADERS FROM "file:///friends.csv" AS row
CALL {
    WITH row
    MATCH (n:User {id: row.friend1}), (p:User {id: row.friend2})
    CREATE(n)-[:FRIENDS_WITH]->(p)
}