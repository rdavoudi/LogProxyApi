# LogProxyApi
This is a sample solution created regarding the below requested scenario:

Creating a Log Proxy API, which receives log messages and forwards them to third-party API.
Requirements:<br />

⁃ Using .Net Core 3.1 Web Api<br />

⁃ Implementing basic authentication to your methods<br />

⁃ Implementing POST /messages, which receives simple JSON object in Body with two attributes "title" and “text" and transfers them to third-party API (see description below) to fields Summary and Message enriching record with generated id and current timestamp<br />

⁃ Implementing GET /messages which returns all the messages from the third-party API

response example should be
[{
    
    "id": "1",
    
    "title": "Test message summary",
    
    "Text": "Exceptiion occured at ...",
    
    "receivedAt": "2021-01-01T00:38:00.000Z"
},
{
    
    "id": "1",
    
    "Title": "Test message summary",
    
    "Text": "Exceptiion occured at ...",
    
    "receivedAt": "2021-01-01T00:38:00.000Z"
}]

⁃ Implementing tests, which you think are needed for this API
