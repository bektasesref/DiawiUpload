# DiawiUpload
Unity Build -> Automatically upload APK files directly to Diawi and send message to Slack

Simple Window tool to automatically upload apk file to Diawi.
Step by Step;

1) Get Diawi Token: Go to your Diawi profile. Enter API Token page. Write a Token Description and click Generate. Copy token and paste it to Unity -> BE -> Uploader -> Diawi Token field
2) Copy Hosted_PHP -> Upload.php file to your webserver. With the new absolute path of php file (for example; somewebsite.com/Uploader/Upload.php), write it to  Unity -> BE -> Uploader -> URL to Trigger
3) If you want Slack Message itnegratino too, Open Slack's Workflow Builder. Create a WebHook with a variable of "data" as you can see in below:
![image](https://github.com/bektasesref/DiawiUpload/assets/23198585/6b5639a7-9016-48d7-9bd0-3d7540c7f33b)
Copy WebHook URL and write it to  Unity -> BE -> Uploader -> Slack Webhook


![image](https://github.com/bektasesref/DiawiUpload/assets/23198585/8cf1ba33-bd50-4657-9129-e65e6701d91a)

If everything is okay, it should be something like this;
![image](https://github.com/bektasesref/DiawiUpload/assets/23198585/4633f188-8697-4da5-8371-417305994bd5)
