<template>
  <div class="center" style="width: 1024px; text-align: center;">
    <img src="../components/Icons/logo-websites-31268.png" style="width =200px; height: 200px;">

    <v-card class="mx-auto pa-8 pb-8" elevation="4" max-width="448" rounded="lg">
      <v-toolbar color="primary">
        <v-toolbar-title>Login</v-toolbar-title>
      </v-toolbar>
      <br />
      <div class="text-subtitle-1 text-medium-emphasis">Account</div>

      <v-text-field density="compact" placeholder="Email address" prepend-inner-icon="mdi-email-outline"
                    variant="outlined"></v-text-field>

      <div class="text-subtitle-1 text-medium-emphasis d-flex align-center justify-space-between">
        Password

        <a class="text-caption text-decoration-none text-blue" href="#" rel="noopener noreferrer" target="_blank">
          Forgot login password?
        </a>
      </div>

      <v-text-field :append-inner-icon="visible ? 'mdi-eye-off' : 'mdi-eye'" :type="visible ? 'text' : 'password'"
                    density="compact" placeholder="Enter your password" prepend-inner-icon="mdi-lock-outline" variant="outlined"
                    @click:append-inner="visible = !visible"></v-text-field>

      <v-btn block class="mb-8" color="blue" size="large" variant="tonal">
        Log In
      </v-btn>

      <v-card-text class="text-center">
        <a class="text-blue text-decoration-none" href="#" rel="noopener noreferrer" target="_blank">
          Sign up now <v-icon icon="mdi-chevron-right"></v-icon>
        </a>
      </v-card-text>
    </v-card>
  </div>
</template>

<script>
  export default {    
    data: () => ({
      visible: false,
    }),
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

