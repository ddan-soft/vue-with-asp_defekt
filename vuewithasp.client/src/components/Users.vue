<template>
<v-table fixed-header>
  <thead>
    <tr>
      <th class="text-left">
        Login name
      </th>
      <th class="text-left">
        User Id
      </th>
    </tr>
  </thead>
  <tbody>
    <tr v-for="item in users"
        :key="item.loginName">
      <td>{{ item.loginName }}</td>
      <td>{{ item.userId }}</td>
    </tr>
  </tbody>
</v-table>
</template>

<script>
  export default {
    data: () => ({
      //users: [{userId: "Test", loginName: "TTest",}],
      users: [],
    }), // ** END -- data
    created() {
      this.fetchData();
    },
    methods: {
      async fetchData() {
        const jsonData = {
          action: "GetUsers",
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
        
        const myRequest = new Request("/run");
        fetch(myRequest, myInit)
          .then((response) => {
            //console.log("response:" + response);
            let vJson = response.json();
            return vJson
          })
          .then((jsonString) => {
            jsonString = jsonString.replace(/'/g, '"');
            console.log("jsonString:" + jsonString)
            const jsonObject = JSON.parse(jsonString);
            if (jsonObject.success === true) {
              this.users = jsonObject.users;
            }
          })
          .catch((error) => {
            console.error(error);
          });
      }
    },
  }
</script>