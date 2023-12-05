## How to handle api credentials in WpEngine/Wordpress

Api Environment Variables are deployed manually onto WpEngine by placing the file `.api-env-vars.php` at the server root.

A template of this file should be sitting at the root of this repo.

the `.api-env-vars.php` is included in the `wp-config.php` file with the following line

```
include '.api-env-vars.php';
```


These environment variables are used in the theme file `sample.php`


## WpEngine Backups

Details: https://wpengine.com/support/restore/ 