<template>
  <v-layout class="rounded rounded-md">
    <v-navigation-drawer>
      <v-card class="mx-auto">
        <v-list class="my-0">
          <v-list-item prepend-icon="mdi-home" title="Home"></v-list-item>

          <v-list-group value="Apps">
            <template v-slot:activator="{ props }">
              <v-list-item v-bind="props"
                           title="Apps"></v-list-item>
            </template>
            <v-list-item v-for="([title, icon], i) in apps"
                         :key="i"
                         :value="title"
                         :prepend-icon="icon"
                         @click="menuOptionClick(title)">
              <v-list-item-title v-html="title" class="wrap-text">

              </v-list-item-title>
            </v-list-item>
          </v-list-group>

          <v-list-group value="System">
            <template v-slot:activator="{ props }">
              <v-list-item v-bind="props"
                           title="System"></v-list-item>
            </template>

            <v-list-item v-for="([title, icon], i) in systemOptions"
                         :key="i"
                         :value="title"
                         :title="title"
                         :prepend-icon="icon"
                         @click="menuOptionClick(title)">
            </v-list-item>
          </v-list-group>

          <v-list-group value="Data">
            <template v-slot:activator="{ props }">
              <v-list-item v-bind="props"
                           title="Data"></v-list-item>
            </template>

            <v-list-item v-for="([title, icon], i) in dataOptions"
                         :key="i"
                         :value="title"
                         :title="title"
                         :prepend-icon="icon"
                         @click="menuOptionClick(title)">
            </v-list-item>
          </v-list-group>
        </v-list>
      </v-card>
    </v-navigation-drawer>

    <v-main class="d-flex align-top justify-tpp" style="min-height: 300px;">
      <router-view v-if="showUsersTable"></router-view>
    </v-main>
  </v-layout>
</template>

<script>
  import Users from '../../components/Users.vue'

  export default {
    data: () => ({
      showUsersTable:false,
      open: ['Menu'],
      apps: [
        ['Working time measurement', 'mdi-clock-time-nine-outline'],
        ['Settings', 'mdi-cog-outline'],
      ],
      systemOptions: [
        ['Users', 'mdi-account-multiple-outline', false],
        ['Sessions', 'mdi-file-table-box-outline', false],
      ],
      dataOptions: [
        ['Persons', 'mdi-account-multiple-outline'],
        ['Cities', 'mdi-city-variant-outline'],
        ['Streets', 'mdi-routes'],
      ],
    }), // ** END -- data
    methods: {
      menuOptionClick(title) {
        switch (title) {
          case 'Users':
            this.showUsersTable = true;
            //this.$router.push({ name: 'users' })
            break;
          default:
            this.showUsersTable = false;
            break;
        }
      },
    },
  }
</script>

<style scoped>
  .wrap-text {
    white-space: normal;
  }
</style>

