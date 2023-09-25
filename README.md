# pdf-GPT
pdf-GPT is a full-stack application built with React and ASP.NET Core that allows users to upload PDFs 
for analysis through token vectorization. This enables users to interact with the contents of the PDF through a 
GPT-like experience, without the need to copy and paste the PDF into a chat interface

# Features
- Upload PDF files for analysis
- Token vectorization of PDF content
- GPT-like interaction with PDF content

# Installation & Setup
**Prerequisites**
- .NET 7.0 SDK
- npm
- Node.js
- docker
- npx

**Steps**
  1. clone the repository
```
git clone https://github.com/tarnar114/pdf-GPT/
```
  2. Navigate to the ClientApp directory and install the npm packages
```
docker run -d --name redis-stack-server -p 6379:6379 redis/redis-stack-server:latest
```
  3. Navigate to the ClientApp directory and install the npm packages
```
cd ClientApp
npm install
```
  4. Navigate back to the root directory and run the application
  ```
  cd ..
  dotnet run
  ```

# Usage
After starting the application, users will need to input their own OpenAI api keys. Once inputted, they can click the bottom left button for file upload in which they choose the file, then they will be able to chat with the bot asking
simple questions about the contents of the pdf

# Future Plans
- Improve text chunking
- Setup chat streaming completion
- UI updates
- Allow for user to chat with multiple files

