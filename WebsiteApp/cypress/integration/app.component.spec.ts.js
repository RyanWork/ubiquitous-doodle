describe('Form field tests', () => {
  beforeEach(() => {
    cy.visit("localhost:4200")
  });

  it('should have the submit button disabled initially', () => {
    cy.get('button').should('be.disabled')
  });

  it('should keep the button disabled if only an email is entered', () => {
    cy.get('input[formControlName="email"]')
      .type("testing@test.com")

    cy.get('button').should('be.disabled')
  });

  it('should keep the button disabled if only a body is entered', () => {
    cy.get('textarea[formControlName="emailBody"]')
      .type("Hello world!")

    cy.get('button').should('be.disabled')
  });

  it('should keep the button disabled if an email is not valid email format and a body is entered', () => {
    cy.get('input[formControlName="email"]')
      .type("someInvalidEmailFormat")
    cy.get('textarea[formControlName="emailBody"]')
      .type("Hello world!")

    cy.get('button').should('be.disabled')
  });

  it('should enable the button if a valid email and body are entered', () => {
    cy.get('input[formControlName="email"]')
      .type("testing@test.com")
    cy.get('textarea[formControlName="emailBody"]')
      .type("Hello world!")

    cy.get('button').should('be.enabled')
  });
});

describe('Link tests', () => {
  beforeEach(() => {
    cy.visit("localhost:4200")
  });

  it('should open https://www.linkedin.com/in/ryanha1996/ when the linkedin button is clicked', () => {
    cy.get('a[title="LinkedIn"')
      .should('have.attr', 'href', 'https://www.linkedin.com/in/ryanha1996/')
  })

  it('should open https://www.github.com/RyanWork when the github link is clicked', () => {
    cy.get('a[title="GitHub"')
      .should('have.attr', 'href', 'https://www.github.com/RyanWork')
      .then(link => {
        cy.request(link.prop('href'))
          .its('status')
          .should('eq', 200)
      })
  })
})
