FROM neo4j:enterprise

ENV NEO4J_ACCEPT_LICENSE_AGREEMENT yes

COPY extension_script.sh /extension_script.sh
COPY second-script.sh /second-script.sh
COPY data/users.csv /import/users.csv
COPY data/friends.csv /import/friends.csv
COPY entities.cql /entities.cql
COPY relationships.cql /relationships.cql

ENV NEO4J_AUTH=neo4j/password
ENV EXTENSION_SCRIPT=/extension_script.sh