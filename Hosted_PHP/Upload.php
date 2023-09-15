<?php

$mailCallback = '';
$slackWebHookInputVar = 'data';	
	
$fileContent = file_get_contents("php://input");

if ($fileContent !== false) 
{
	$buildName = "Build.apk";
	$notifyOnSlack = $_GET['notifyOnSlack'];
	$diawiToken = $_GET['diawiToken'];
	$slackWebHook = $_GET['slackWebHook'];
    $file = fopen($buildName, "wb");
    if ($file !== false) {
        if (fwrite($file, $fileContent) !== false) 
		{
            fclose($file);
            echo "APK file uploaded successfully\n";

			
			$url = "https://upload.diawi.com/"; 
			$filename = realpath($buildName);


			$headers = array("Content-Type: multipart/form-data");
			$postfields = array(
				"token"             => $diawiToken,
				"file"              => new CurlFile( $filename ),
				"find_by_udid"      => 0,
				"wall_of_apps"      => 0,
				"callback_email"    => $mailCallback
				);
			$ch = curl_init();
			$options = array(
				CURLOPT_URL => $url,
				CURLOPT_POST => 1,
				CURLOPT_HTTPHEADER => $headers,
				CURLOPT_POSTFIELDS => $postfields,
				CURLOPT_USERAGENT => 'Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:47.0) Gecko/20100101 Firefox/47.0',
				CURLOPT_RETURNTRANSFER => 1
			);
			curl_setopt_array($ch, $options);
			$output = curl_exec($ch);
			$info = curl_getinfo($ch);
			curl_close($ch);


			$job_details = json_decode($output); //the response is in json format
			$job_id = $job_details->job;
			$status_link = "https://upload.diawi.com/status?token=".$diawiToken."&job=".$job_id;

			$done = false;
			$links;
			while(!$done)
			{
				$ch = curl_init();
				curl_setopt($ch, CURLOPT_URL, $status_link);
				curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
				$output = curl_exec($ch);
				curl_close($ch);
				
				$status = json_decode($output); //the response is in json format
				$status_code = $status->status;
				if ($status_code == "2001")
					sleep(2);
				else
				{
					$links = json_encode($status->links, JSON_UNESCAPED_SLASHES);
					$done = true;
				}
			}

			if ($notifyOnSlack == "true")
			{
				$message = json_encode(array($slackWebHookInputVar => $links));
				$c = curl_init($slackWebHook);
				curl_setopt($c, CURLOPT_SSL_VERIFYPEER, false);
				curl_setopt($c, CURLOPT_POST, true);
				curl_setopt($c, CURLOPT_POSTFIELDS, $message);
				curl_exec($c);
				curl_close($c);
			}
			print_r($links);
        } 
		else
            echo "Error writing to file.";
    }
	else
        echo "Error opening file for writing.";
} 
else
    echo "Error reading input.";
?>