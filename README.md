## Setup

1. Run the bank simulator by executing the following command:
   ```bash
   docker-compose up
2. Navigate to the src/PaymentGateway.Api directory:
    ```bash
   cd src/PaymentGateway.Api
3. Start the application using the .NET CLI:
     ```bash
   dotnet run
4. Access the API overview through Swagger at: https://localhost:7092/swagger/index.html


## Application structure

`/Common` - Contains shared code that can be used across different parts of the project. 

`/Controllers` - Contains the API controllers and simple validation rules for user input.

`/DAL` - Data Access Layer, contains repositories for data management.

`/Domain` - Contains domain classes representing the core business logic of the application.

`/Enums` - Contains enumerations used throughout the application.

`/Infrastructure` - Registers various services in the Dependency Injection (DI) container, defines mappings.

`/Integrations` - Contains everything related to the integration with third-party services.

`/Models` - Contains request and response models for data transfer between different layers of the application.

`/Services` - Contains services that implement business logic and validation rules at the business logic layer.

## Key design considerations and assumptions

- **Singletons were used for simplicity**   
  In a more complex application or a real-world scenario, the use of singletons might be reconsidered.
- **No real database is used.**

- **No authentication is implemented.**

- **No retry mechanism is implemented, meaning a transaction may be attempted any number of times.**

- **When a "Rejected" status is returned, the user receives all the information they provided, including the masked card number.**  
    Providing all the information when the payment is rejected increases user convenience. For example, the user can see exactly what data was submitted, which helps in identifying mistakes.  

- **Payments rejected at the validation stage are not saved to the database.**  
  However, all attempts to make a payment that were sent to the bank are saved.
- **Payments that are neither "authorized" nor "declined" are considered "rejected" as they violated some validation rules from the bank.**  
    This rule is consistent with the requirements in this task. However, depending on the specifics of the task or different bank rules, this logic might change.

- **The user can retrieve payment details for all statuses (authorized, declined, rejected).**
    However, not displaying "rejected" payments and storing them only for internal use would be also a valid option.

## In an ideal world, the following things could be added or changed in the application:

- **API versioning:**  
  It's a good practice to implement versioning in an API to ensure backward compatibility when the API evolves over time.

- **Return validation error details:**  
  When validation errors occur, returning detailed error messages can significantly improve the user experience and make it easier to debug issues.

- **Clear separation of models for each application layer (data layer/domain, business layer, web API):**  
  It's important to have clear separation between models used in different layers of the application to ensure maintainability and flexibility.

- **Containerize the application:**

- **Authentication and database integration:**  
This would be essential for any production-ready payment gateway.

- **Logging:**

- **Improve user input validation:**

- **Add monitoring and audit logs for security violations:**  
  Monitoring and audit logs are critical for tracking suspicious activity and ensuring compliance with security standards. Implementing monitoring for unusual payment patterns, failed login attempts, and other potential security violations would help protect against fraud and unauthorized access.

## Tests
Unit tests have been implemented for the controller, but tests for basic flows across the entire application, integration tests, as well as additional unit tests (such as validation tests, edge case tests, etc.) are necessary.




