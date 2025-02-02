# 🚀 gRPC  

## 📌 O que é gRPC?  
gRPC (Google Remote Procedure Call) é um framework moderno de comunicação entre serviços baseado em **Protocol Buffers (Protobuf)** e **HTTP/2**. Ele é mais rápido e eficiente que REST e SOAP, sendo ideal para microsserviços.  

## 🔥 Principais Vantagens  
✅ **Alto desempenho** (binário e HTTP/2)  
✅ **Baixa latência** (ótimo para comunicação entre serviços)  
✅ **Streaming** (suporte a fluxo contínuo de dados)  
✅ **Tipagem forte** (gera código automaticamente a partir do `.proto`)  
✅ **Multiplataforma** (funciona em .NET, Java, Python, Go e mais)  

## 🔄 Modos de Comunicação  
1. **Unary** → Cliente envia 1 requisição e recebe 1 resposta  
2. **Server Streaming** → Cliente envia 1 requisição, servidor envia várias respostas  
3. **Client Streaming** → Cliente envia várias requisições, servidor responde uma vez  
4. **Bidirectional Streaming** → Cliente e servidor trocam múltiplas mensagens ao mesmo tempo  

## 🛠️ Tecnologias Utilizadas  
- **Protocol Buffers (Protobuf)** → Formato binário eficiente  
- **HTTP/2** → Melhor performance e multiplexação  
- **TLS/SSL** → Segurança integrada  
- **Interceptors** → Para autenticação, logging e monitoramento  

## 📚 Aplicações do gRPC  
- 🔹 Microsserviços  
- 🔹 Comunicação entre servidores  
- 🔹 Streaming de dados em tempo real  
- 🔹 IoT e Edge Computing  

## 🔗 Referência Oficial  
[grpc.io](https://grpc.io/) | [Microsoft Docs](https://learn.microsoft.com/en-us/aspnet/core/grpc/)  

