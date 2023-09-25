using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class menucontroller : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_Text feedbackText;  // Drag your feedback message Text here
    private string email;
    private string password;

    private async void Start()
    {
        await TestConnection();
    }
    void Update()
    {
        email = emailInput.text;
        password = passwordInput.text;
        //TestConnection();

    }
    // Just test connection to the server
    public async Task TestConnection()
    {
        string testUrl = "http://localhost:4444"; // Replace with the API URL that returns 'hello!'

        using (HttpClient client = new HttpClient())
        {
            try
            {
                // Send a GET request
                HttpResponseMessage response = await client.GetAsync(testUrl);

                // Check if the request was successful (status code 200 OK)
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content
                    string message = await response.Content.ReadAsStringAsync();

                    if (message == "hello!")
                    {
                        Debug.Log("Connection successful!");
                    }
                    else
                    {
                        Debug.Log("Connection test failed: Unexpected response.");
                    }
                }
                else
                {
                    Debug.Log("Error: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Exception Error: " + ex.Message);
            }
        }
    }
    public void login()
    {
        loginAsync();
    }

    public void signup()
    {
        signupAsync();
    }

    public async void loginAsync()
    {
        Debug.Log("Button LogIn!");
        Debug.Log(email + " - " + password);
        string apiUrl = "http://localhost:4444/login"; // Replace with your actual API URL

        // Create a JSON object with email and password
        var loginData = new
        {
            email = email,
            password = password
        };

        // Serialize the loginData object to JSON
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(loginData);

        // Create an instance of HttpClient
        using (HttpClient client = new HttpClient())
        {
            // Create a StringContent with the JSON data
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                // Send the POST request
                HttpResponseMessage response = await client.PostAsync(apiUrl, payload);

                // Check if the request was successful (status code 200 OK)
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content (assuming it's a boolean value)
                    bool loginSuccess = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());

                    if (loginSuccess)
                    {
                        Debug.Log("Login successful!");
                        
                        //feedbackText.text = "Login successful!"; //dehighligh for use
                        SceneManager.LoadScene("Playground");

                    }
                    else
                    {
                        Debug.Log("Login failed: Invalid credentials.");
                        //feedbackText.text = "Login failed: Invalid credentials.";  //Unhighlight for use
                    }
                }
                else
                {
                    Debug.Log("Error: " + response.StatusCode);
                    // Handle other error cases
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Exception Error: " + ex.Message);
                // Handle exceptions
            }
        }
    }

    public async void signupAsync()
    {
        //todo: Implement signup functionality
        Debug.Log("Button Press: SignUp");
        Debug.Log(email + " - " + password);
        string apiUrl = "http://localhost:4444/register"; // Replace with your actual API URL

  

        // Create a JSON object with email and password
        var registerData = new
        {
            email = email,
            password = password
        };

        // Serialize the loginData object to JSON
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(registerData);

        // Create an instance of HttpClient
        using (HttpClient client = new HttpClient())
        {
            // Create a StringContent with the JSON data
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                // Send the POST request
                HttpResponseMessage response = await client.PostAsync(apiUrl, payload);

                // Check if the request was successful (status code 200 OK)
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content (assuming it's a boolean value)
                    bool registerSuccess = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
                    Debug.Log("Rececive messeage:" + registerSuccess);
                    
                    if (registerSuccess)
                    {
                        Debug.Log("Register successful!");
                        // If register is successful
                        
                    }
                    if (!registerSuccess)
                    {
                        Debug.Log("Register failed: User existed!");
                        // Handle the case where register failed due to invalid credentials
                    }
                }
                else
                {
                    Debug.Log("Error: " + response.StatusCode);
                    // Handle other error cases
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Exception Error: " + ex.Message);
                // Handle exceptions
            }
        }
    }
}