FROM neo4j:enterprise

ENV NEO4J_ACCEPT_LICENSE_AGREEMENT yes

COPY my-entrypoint.sh /my-entrypoint.sh
COPY /data /import
RUN chmod +x /my-entrypoint.sh

ENTRYPOINT [ /my-entrypoint.sh ]