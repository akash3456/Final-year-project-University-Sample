using System;
using TechTalk.SpecFlow;

namespace PDFGenerator
{
    [Binding]
    public class ValidateEmailAddressesSteps
    {
        [Given(@"an email address entered in a spreadsheet must be a \.com or \.ac\.uk or \.co\.uk domain")]
public void GivenAnEmailAddressEnteredInASpreadsheetMustBeA_ComOr_Ac_UkOr_Co_UkDomain()
{
    ScenarioContext.Current.Pending();
}

        [When(@"you enter a \.man email address")]
public void WhenYouEnterA_ManEmailAddress()
{
    ScenarioContext.Current.Pending();
}

        [Then(@"the result should be fail\.")]
public void ThenTheResultShouldBeFail_()
{
    ScenarioContext.Current.Pending();
}
    }
}
