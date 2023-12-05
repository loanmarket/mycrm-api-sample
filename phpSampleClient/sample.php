<?php
    /**
     * Request Token
     */
    public static function getAuthToken()
    {
            $clientId = CLIENT_ID;
            $clientSecret = CLIENT_SECRET;
            $authBasic = base64_encode($clientId.':'.$clientSecret);
            $data = http_build_query([
                'grant_type' => 'client_credentials',
                'scope' => 'api.advisers.read api.advisers.search api.adviserdetails.read api.adviserdetails.search',
            ]);

            $headers = [
                'Authorization: Basic '.$authBasic,
                'Accept: application/vnd.api+json',
                'Cache-Control: no-cache',
            ];

            $authUrl = AUTH_URL;
            $ch = curl_init();
            curl_setopt($ch, CURLOPT_URL, $authUrl);
            curl_setopt($ch, CURLOPT_POST, true);
            curl_setopt($ch, CURLOPT_POSTFIELDS, $data);
            curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
            curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);
            $response = curl_exec($ch);
            $error_msg = curl_error($ch);
            $statusCode = curl_getinfo($ch, CURLINFO_HTTP_CODE);
            curl_close($ch);

            if ($statusCode !== 200) {
                echo 'Error: '.$statusCode.$error_msg;
            // Log error or handle the failure
            } else {
                static::$auth = $cred = json_decode($response, true);
                static::$auth['last_updated'] = time();
            }
    }

    /**
     * Call API to given endpoint
     *
     * @param  string  $endpoint
     * @param  array  $params
     */
    public static function getbrokerDetails()
    {
        $current_page = 1;
        $endpoint = '/jsonapi/adviser-details?page[number]='.$current_page.'&include=adviser,adviser.organisation,adviser.organisation.addresses';
        getAuthToken();
        $baseUrl = BASE_URL;
        $apiUrl = $baseUrl.$endpoint;

        $firstName = "Joseph";
        $lastName = "Bloggs";
        $preferredName "Joe Bloggs";


        $filterStr = '';
        $filterStr .= ($k > 0 ? ',': '').
            'or(contains(firstName,\''.trim($firstName).
            '\'),contains(preferredName,\''.trim($preferredName).
            '\'),contains(lastName,\''.trim($lastName).
            '\'))';                
        // FLAG TO CHECK A BROKER HAS CONSENTED TO BEING DISPLAYED IN PUBLIC SEARCH
        $filterStr = 'and('.$filterStr.',equals(adviser.isBrokerWebPublic,\'true\'))';
        $apiUrl .= '&filter='.$filterStr;
        
        $headers = [
            'Authorization: Bearer '.static::$auth['access_token'],
            'Accept: application/vnd.api+json',
        ];

        // Initialize cURL session
        $ch = curl_init();
        curl_setopt($ch, CURLOPT_URL, $apiUrl);
        curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);
        curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
        $response = curl_exec($ch);
        $error_msg = curl_error($ch);
        $statusCode = curl_getinfo($ch, CURLINFO_HTTP_CODE);
        curl_close($ch);
        // Check for errors
        if ($statusCode !== 200) {
            echo 'Request Error: '.$statusCode.$error_msg;
        } else {
            // Parse the JSON response
            $return = json_decode($response, true);
            return $return;
        }
    }
