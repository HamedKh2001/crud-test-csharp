Feature: Delete Customer Command Handling

  Scenario: Deleting an existing customer
    Given a customer repository
    And an existing customer with the ID
      | Id                                     |
      | 7d94a0f4-29cc-4e9c-9c55-70581229f22d    |
    When the delete customer command is handled
    Then the customer with the specified ID should be retrieved from the repository
    And the customer should be deleted from the repository
