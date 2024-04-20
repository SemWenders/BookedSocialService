load() {
    echo "Loading data"
    neo4j-admin import --nodes=import/users.csv --relationships=import/friends.csv neo4j
}

load