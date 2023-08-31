Feature: Create Customer Command Handling

  Scenario: Creating a new customer
    Given a customer repository
    And a customer creation command
      | FirstName | LastName | Email             | DateOfBirth   |
      | hamed      | kh      | hamed.30sharp@gmail.com | 2001, 2, 10    |
    When the create customer command is handled
    Then a new customer should be created with the provided details
    And the customer should be saved in the repository
    And the corresponding event should be saved in the event source
