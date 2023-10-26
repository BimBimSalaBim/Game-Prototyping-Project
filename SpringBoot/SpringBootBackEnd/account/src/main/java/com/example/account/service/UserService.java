package com.example.account.service;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.stereotype.Service;
import com.example.account.repository.UserRepository;
import com.example.account.entity.User;

@Service
public class UserService {

    @Autowired
    private UserRepository repo;

    private final BCryptPasswordEncoder passwordEncoder = new BCryptPasswordEncoder();

    public void save(User user) {
        user.setPassword(passwordEncoder.encode(user.getPassword())); // Hashing the password before saving
        repo.save(user);
    }

    public boolean findUserByEmail(String email) {
        User user = repo.findByEmail(email);
        return user != null;
    }

    public User lookForUserByEmail(String email) {
        return repo.findByEmail(email);
    }

    public boolean findUser(String email, String password) {
        User user = repo.findByEmail(email);
        if (user != null) {
            return passwordEncoder.matches(password, user.getPassword()); // Matching user's entered password with the stored hashed password
        }
        return false;
    }
}
