package com.sdmay2144.backend.controllers;

import com.sdmay2144.backend.models.User;
import com.sdmay2144.backend.services.interfaces.UserService;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping(path="/user")
public class UserController {
    private UserService userService;

    public UserController(UserService userService) {
        this.userService = userService;
    }

    @PostMapping(path="/add")
    public @ResponseBody String addUser (@RequestParam String firstName,
                                         @RequestParam String middleName,
                                         @RequestParam String lastName,
                                         @RequestParam String address,
                                         @RequestParam String phoneNumber,
                                         @RequestParam String username,
                                         @RequestParam String password)
    {//Need to add validation techniques for all these variables
        StringBuilder errorMsg = new StringBuilder();
        if(firstName == null || firstName.isEmpty()) {
            errorMsg.append("First name cannot be null or empty ");
        }
        if(middleName == null) {
            errorMsg.append("Middle name cannot be null ");
        }
        if(lastName == null || lastName.isEmpty()) {
            errorMsg.append("Last name cannot be null or empty");
        }
        if(address == null || address.isEmpty()) {
            errorMsg.append("Address cannot be null or empty");
        }
        if(phoneNumber == null || phoneNumber.isEmpty()) {
            errorMsg.append("Phone number cannot be null or empty");
        }
        if(username == null || username.isEmpty()) {
            errorMsg.append("Username cannot be null or empty");
        }
        if(password == null || password.isEmpty()) {
            errorMsg.append("Password cannot be null or empty");
        }
        if(errorMsg.length() > 0) {
            errorMsg.deleteCharAt(errorMsg.length()-1);//Remove ending space;
            throw new IllegalArgumentException(errorMsg.toString());
        }
        User newUser = new User();
        newUser.setFirstName(firstName);
        newUser.setMiddleName(middleName);
        newUser.setLastName(lastName);
        newUser.setAddress(address);
        newUser.setPhoneNumber(phoneNumber);
        newUser.setUsername(username);
        newUser.setPassword(password);
        newUser.sha1Password();
        if(userService.addUser(newUser) != null) {
            return "success";
        }
        return "Failed to insert user";
    }

    @GetMapping("/all")
    public @ResponseBody Iterable<User> getAllUsers() {
        return userService.getAllUsers();
    }

    @PostMapping("/login")
    public @ResponseBody boolean login(@RequestParam String username, @RequestParam String password) {
        User loginUser = new User();
        loginUser.setUsername(username);
        loginUser.setPassword(password);
        loginUser.sha1Password();
        return userService.login(loginUser) != null;
    }
}
