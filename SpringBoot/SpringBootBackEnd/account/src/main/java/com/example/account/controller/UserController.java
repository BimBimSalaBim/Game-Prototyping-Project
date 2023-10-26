package com.example.account.controller;

import com.example.account.entity.User;
import com.example.account.service.MyService;
import com.example.account.service.UserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.util.Date;
@RestController

public class UserController {
    private static final Logger logger = LoggerFactory.getLogger(MyService.class);
    @Autowired
    private UserService service;


    @PostMapping("/register")
    public ResponseEntity<Boolean> register(@RequestBody User user) {
        // Set the creation date when saving a new user
        logger.info("Someone using /register");
        String email = user.getEmail();
        boolean userFound = service.findUserByEmail(email);
        if (!userFound){
            if (user.getId() == null) {
                user.setCreatedAt(new Date());
                service.save(user);
                logger.info("New user: " + user.getEmail() + " created!");
                return ResponseEntity.ok(true);
            }
        }
        else{
            logger.info("Query result: " + userFound + " User existed!");
            // Return true if the user is found, false otherwise
            return ResponseEntity.ok(false);
        }
        return ResponseEntity.ok(false);
    }


    @PostMapping("/login")
    public ResponseEntity<Boolean> login(@RequestBody User user) {
        //Always return true for testing
        logger.info("Someone using /login");
        logger.info("JSON received: " + user.toString());
        String email = user.getEmail();
        String password = user.getPassword();
        boolean userFound = service.findUser(email, password);
        logger.info("Query result: " + userFound);
        // Return true if the user is found, false otherwise
        return ResponseEntity.ok(userFound);

    }

    @GetMapping
    public String greet() {
        return "hello!";
    }


}
