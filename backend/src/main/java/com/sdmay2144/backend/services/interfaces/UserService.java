package com.sdmay2144.backend.services.interfaces;

import com.sdmay2144.backend.models.User;

import java.util.List;

public interface UserService {
    List<User> getAllUsers();
    User addUser(User u);
}
