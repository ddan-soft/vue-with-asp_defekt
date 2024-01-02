<template>
  <div class="center" style="width: 1024px; text-align: center;">
    <img src="../components/Icons/logo-websites-31268.png" style="width:200px; height: 200px;">
    <v-form v-model="valid">
      <v-card class="mx-auto pa-8 pb-8" elevation="4" max-width="448" rounded="lg">
        <v-toolbar color="primary">
          <v-toolbar-title>Login</v-toolbar-title>
        </v-toolbar>
        <br />
        <br />

        <v-text-field v-model="username"
                      ref="userName"
                      required
                      :rules="nameRules"
                      density="compact" label="User name"
                      prepend-inner-icon="mdi-account-outline"
                      variant="outlined">
        </v-text-field>
        <br />
        <br />

        <div class="text-subtitle-1 text-medium-emphasis d-flex align-center justify-space-between">
          <a v-if="false" class="text-caption text-decoration-none text-blue" href="#" rel="noopener noreferrer" target="_blank">
            Forgot login password?
          </a>
        </div>

        <v-text-field v-model="password"
                      :rules="passsRules"
                      required
                      :append-inner-icon="visible ? 'mdi-eye-off' : 'mdi-eye'"
                      :type="visible ? 'text' : 'password'"
                      density="compact" label="Enter your password"
                      prepend-inner-icon="mdi-lock-outline" variant="outlined"
                      @click:append-inner="visible = !visible">

        </v-text-field>
        <br />
        <br />
        <br />

        <v-btn @click="logIn"
               style="border: 1px solid blue;"
               block class="mb-8"
               color="blue" size="large" rounded="xl">
          Log In
        </v-btn>
        <p>{{loginResult}}</p>

        <v-card-text v-if="false" class="text-center">
          <a class="text-blue text-decoration-none" href="#" rel="noopener noreferrer" target="_blank">
            Sign up now <v-icon icon="mdi-chevron-right"></v-icon>
          </a>
        </v-card-text>
      </v-card>
    </v-form>
  </div>
</template>

<script>
  export default {
    data: () => ({
      visible: false,
      valid: false,
      username: '',
      password: '',
      loginResult: '',
      nameRules: [
        value => {
          if (value) return true

          return 'Name is required.'
        },
      ],
      passsRules: [
        value => {
          if (value) return true
          return 'Password is required.'
        },
      ],
    }), // ENDE -- data:
    mounted() {
      this.$refs["userName"].focus();
    },
    methods: {
      async logIn() {
        const jsonData = {
          userName: escape(this.username.replace(/[&<>"'\/]/g, '')),
          password: escape(this.password.replace(/[&<>"'\/]/g, ''))
        };

        const postData = new FormData();
        postData.append("json", JSON.stringify(jsonData));

        const myHeaders = new Headers();

        const myInit = {
          method: "POST",
          headers: myHeaders,
          mode: "cors",
          cache: "default",
          body: postData
        };

        const myRequest = new Request("login");

        fetch(myRequest, myInit)
          .then((response) => {
            //console.log("response:" + response);
            let vJson = response.json();
            return vJson
          })
          .then((jsonString) => {
            jsonString = jsonString.replace(/'/g, '"')
            const jsonObject = JSON.parse(jsonString);
            
            this.loginResult = jsonObject.message
            if (jsonObject.success === true) {
              sessionStorage.setItem("Page_Home_Session_Id", jsonObject.sessionId)
              window.location.href = '/src/pages/home/home.html';
            }
          })
          .catch((error) => {
            console.error(error);
          });
        ;

      }
    }
  }
</script>

<style scoped>
  .center {
    border: none;
    position: absolute;
    top: 50%;
    transform: translate(-50%, -50%);
    padding: 10px;
  }
</style>

