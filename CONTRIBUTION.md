# How to contribute to AuthyDecryptor

Everyone is allowed to contribute to this repository - all kind of contribution is welcome.

### Obtain Syncfusion license
As first step, **everyone** developing the software has to obtain their own Syncfusion license. You may choose between a [Community License](https://www.syncfusion.com/products/communitylicense) or a [commercial license](https://www.syncfusion.com/sales/teamlicense). For more information about the Syncfusion licensing, take a look at the [SYNCFUSION-LICENSE](SYNCFUSION-LICENSE) file.

After obtaining a license, create an environment variable in your users context with the name "PasswordChecker_SyncFusion_License". As value, save the license key. Make sure that the generated license key matches the currently used SyncFusion version **24.2.6**.

## Did you find a bug?
* **Do not open an issue if the bug ins in the Netwrix Password Secure SDK/API!** Instead, report this to their Support Team.
* Ensure that the issue is not yet reported. For this, use the [issues tab](https://github.com/florian-berger/AuthyDecryptor/issues) of this repository.
* If there is already an issue closed with the hint that it won't be fixed, **do not open a new one**! It won't be fixed then.
* If you can't find an issue in the repo, create a new one. Use a **clear title** and **description**, add as much information as possible.

## Contribute to code
If you contribute with writing code, please note the following points:
* Cosmetic changes of the code that don't add anything substantial in case of stability or functionality will not be accepted in general.
* Also, there are no changes accepted that collects access data, secrets or similar to may lead to a security issue for users!
* Open a new Pull Request on GitHub with the patch or new functionality.
* Make sure that the PR description clearly describes the problem and solution. If there is an issue for this, include it to your description.
* Ensure that you separated checking logic from the UI and CLI part.