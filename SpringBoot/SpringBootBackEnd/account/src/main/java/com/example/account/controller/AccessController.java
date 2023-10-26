package com.example.account.controller;

import com.example.account.service.MyService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.security.PrivateKey;

@RestController
@RequestMapping("/authentication")
public class AccessController {
    private static final Logger logger = LoggerFactory.getLogger(MyService.class);
    @PostMapping("/login")
    public ResponseEntity<String> login(@RequestBody String encryptedChallenge) {
        try {
            // Your authentication logic here
            // If authentication is successful, send a confirmation
            if (authenticationIsSuccessful(encryptedChallenge)) {
                // Send a confirmation message
                logger.info("Login: Successful!");
                return ResponseEntity.ok("Authentication successful. You can now load the new scene.");
            } else {
                logger.info("Login: Fail!");
                return ResponseEntity.status(HttpStatus.UNAUTHORIZED).body("Authentication failed");
            }
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body("Authentication error");
        }
    }

    // Implement your authentication logic here
    private boolean authenticationIsSuccessful(String encryptedChallenge) {
        // Add your logic to validate the encrypted challenge
        // Return true if authentication is successful, false otherwise
        if(encryptedChallenge != "Irregular-Innovations"){
            return false;
        }
        else {
            return true;
        }
    }
}
