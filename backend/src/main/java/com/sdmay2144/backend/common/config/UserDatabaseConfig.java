package com.sdmay2144.backend.common.config;

import org.springframework.context.annotation.Configuration;
import org.springframework.data.jpa.repository.config.EnableJpaRepositories;

@Configuration
@EnableJpaRepositories(basePackages = "com.sdmay2144.backend.repositories")
public class UserDatabaseConfig {
}
