## Backing services

### MongoDB
```
docker run --rm -d -p 27017:27017 -h $(hostname) --name mongo mongo:6.0.5 --replSet=dbrs && sleep 5 && docker exec mongo mongosh --quiet --eval "rs.initiate();"
```

### PostgreSQL
```
docker run -p 5432:5432 --name my-postgres -e 'POSTGRES_USER=postgres' -e 'POSTGRES_PASSWORD=root' -d postgres
docker run -p 5050:80 --name my-pgadmin -e 'PGADMIN_DEFAULT_EMAIL=postgres@test.com' -e 'PGADMIN_DEFAULT_PASSWORD=root' -d dpage/pgadmin4
docker network create --driver bridge pgnetwork
docker network connect pgnetwork my-postgres
docker network connect pgnetwork my-pgadmin
```

### Redis
```
docker run --name redis-server -p6379:6379 -d redis --requirepass "root"
```

### Jaeger
UI at http://localhost:16686
```
docker run --name my-jaeger -p 5775:5775/udp -p 5778:5778 -p 6831:6831/udp -p 6832:6832/udp -p 9411:9411 -p 14268:14268 -p 16686:16686 -d --restart=unless-stopped jaegertracing/opentelemetry-all-in-one
```

### Seq
UI at http://localhost:5341/
```
docker run --name my-seq -e ACCEPT_EULA=y -p 5341:80 -d --restart=unless-stopped datalust/seq
```