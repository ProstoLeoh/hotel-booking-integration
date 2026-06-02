FROM golang:1.21-alpine
WORKDIR /app
COPY go.mod main.go ./
RUN go mod download
RUN go build -o main .
CMD ["./main"]
EXPOSE 5003
