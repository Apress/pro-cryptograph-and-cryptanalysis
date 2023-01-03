[HttpPost]
public ActionResult LogOn(LogOnModel model, string returning_url)
{
    if (ModelState.IsValid)
    {
        if (MembershipService.ValidateUser(model.UserName, model.Password))
        {
            FormsService.SignIn(model.UserName, model.RememberMe);
            if (Url.IsLocalUrl(returning_url)){
                return Redirect(returning_url);
            }
            else {
                return RedirectToAction("Index", "Home");
            }
        }
        else {
            ModelState.AddModelError("", 
        "The credentials are wrong. Please, try again.");
        }
    }
 
    //** if we reach here it means that 
	//** something went wrong
	//** we will show again the form
    return View(model);
}