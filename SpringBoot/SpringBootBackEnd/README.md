
# Google doc version of this document: https://docs.google.com/document/d/1KO_bza_7ZxuJuy7M4lZt8CXBXCuhwV7UxpJyI7PJNR0/edit?usp=sharing
# Spring Boot backend server application.

Github repo: 
Langues using java.

How to run Spring boot in your IDE environment?

I recommend you use Intelij IDE for the Spring boot application.

Before you start please install Amazon Corretto 17 you can find the ink here: Downloads for Amazon Corretto 17 - Amazon Corretto 17

In Windows machine to check your jdk is running in the collect environment please go to System Properties - Environment Variables and check JAVA_HOME is pointing to Correntto 17


After this point, your application is ready for running and development.
 
Few notes and tips for running using Spring Boot application:

For security, sping boot will block every incoming traffic from unknown addresses. So for your front end able to send the method to the backend make sure it is running in webserver.

If you have python3 installed on your machine you can navigate to the root folder of your webpage open the terminal or CMD and use this cml: python3 -m http.server. 

This will start a web server and your default webpage should be named index.html.
To access the webpage open your browser and enter url:  localhost:8000.

I have created an API to take a JSON to send data please use: http://localhost:4444/register 

The backsend server is configured to listen on port 4444 If you want to make a change please go to AccountApplication and change to the server port.
