FROM microsoft/dotnet
ADD src/ /app
WORKDIR /app
RUN dotnet publish --self-contained -r linux-x64 -v q 
EXPOSE 8080
ENTRYPOINT ["./bin/Debug/netcoreapp2.0/linux-x64/fib"] 
