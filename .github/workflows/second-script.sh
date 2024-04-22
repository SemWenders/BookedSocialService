sleep 80
echo "Loading data"
cypher-shell -u neo4j -p password -f /entities.cql
echo "Entities loaded, now loading relationships"
cypher-shell -u neo4j -p password -f /relationships.cql
echo "Data loaded"