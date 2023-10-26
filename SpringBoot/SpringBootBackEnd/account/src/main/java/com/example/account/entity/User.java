package com.example.account.entity;


import org.springframework.data.annotation.CreatedDate;
import org.springframework.data.annotation.Id;
import org.springframework.data.annotation.LastModifiedDate;
import org.springframework.data.mongodb.core.mapping.Document;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.Date;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Document("user")
public class User {

    @Id
    private String id;

    private String email;
    private String password;
    @CreatedDate // This annotation automatically populates the creation date
    private Date createdAt;

    @LastModifiedDate // This annotation automatically updates the last modified date
    private Date updatedAt;


    
}
