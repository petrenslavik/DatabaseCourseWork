<template>
  <div class="container">
    <div class="card border-danger mb-3 rounded col-6">
      <div class="card-header text-center">Registration</div>
      <div class="card-body">
        <form novalidate @submit.prevent="register">
          <div class="input-group mb-3">
            <div class="input-group-prepend">
              <span class="input-group-text" id="fn">First name</span>
            </div>
            <input name="FirstName" type="text" required
                   v-bind:class="[isValidFirstName? 'is-valid': 'is-invalid']"
                   class="form-control"
                   placeholder="Viacheslav"
                   aria-label="First Name"
                   aria-describedby="fn" v-model.trim="firstName">
            <div class="invalid-feedback">
              Please enter a valid name.
            </div>
          </div>
          <div class="input-group mb-3">
            <div class="input-group-prepend">
              <span class="input-group-text" id="sn">Second name</span>
            </div>
            <input name="SecondName" type="text" required
                   :class="[isValidSecondName? 'is-valid': 'is-invalid']"
                   class="form-control" placeholder="Petrenko" aria-label="First Name"
                   aria-describedby="sn" v-model="secondName">
            <div class="invalid-feedback">
              Please enter a valid second name.
            </div>
          </div>
          <div class="input-group mb-3">
            <div class="input-group-prepend">
              <span class="input-group-text" id="email">Email</span>
            </div>
            <input name="Email" type="email" required :class="[isValidEmail? 'is-valid': 'is-invalid']"
                   class="form-control" placeholder="mail@gmail.com"
                   aria-label="Email"
                   aria-describedby="email" v-model="email">
            <div class="invalid-feedback">
              Please enter a valid email.
            </div>
          </div>
          <div class="input-group mb-3">
            <div class="input-group-prepend">
              <span class="input-group-text" id="username">@</span>
            </div>
            <input name="Username" type="text" class="form-control"
                   :class="[isValidUsername? 'is-valid': 'is-invalid']"
                   placeholder="Username" aria-label="Username"
                   aria-describedby="username" v-model="username">
          </div>
          <div class="form-group">
            <input name="Password" required type="password"
                   :class="[isValidPassword? 'is-valid': 'is-invalid']"
                   class="form-control" placeholder="Password" v-model="password"
                   pattern=".{8,}">
            <div class="invalid-feedback">
              Password should contain at least 8 symbols.
            </div>
          </div>
          <div class="form-group">
            <input name="RepeatPassword" required type="password"
                   :class="[isValidRepeatPassword? 'is-valid': 'is-invalid']"
                   class="form-control" placeholder="Repeat password"
                   v-model="repeatPassword">
            <div class="invalid-feedback">
              Passwords don't match.
            </div>
          </div>
          <div class="form-group">
            <input type="hidden" class="form-control is-invalid">
            <div class="invalid-feedback">
              {{errorMessage}}
            </div>
          </div>
          <div class="row">
            <router-link class="col text-center" to="/">
              <button class="w-50 btn btn-danger">Back</button>
            </router-link>
            <div class="col text-center">
              <button class="w-50 btn btn-danger" type="submit">Register</button>
            </div>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>

  export default {
    name: "Register",
    data: function () {
      return {
        firstName: null,
        secondName: null,
        email: null,
        username: null,
        password: null,
        repeatPassword: null,
        errorMessage: null,
      }
    },
    computed: {
      isValidFirstName: function () {
        return !!this.firstName;
      },
      isValidSecondName: function () {
        return !!this.secondName;
      },
      isValidUsername: function () {
        return (!this.username) || (/^[a-zA-Z0-9]+$/.test(this.username));
      },
      isValidPassword: function () {
        return (!!this.password) && this.password.length > 7;
      },
      isValidRepeatPassword: function () {
        return (!!this.password) && this.repeatPassword === this.password;
      },
      isValidEmail: function () {
        var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return (!!this.email) && re.test(this.email);
      }
    },
    methods: {
      register: function (event) {
        var formData = new FormData(event.target);
        this.$store.dispatch("account/Register", formData, {root: true})
          .then(response=>this.successfulRegister(response))
          .catch(error=>this.unsuccessfulRegister(error));
      },

        successfulRegister: function () {
          this.errorMessage = null;
          this.$router.push("/confirmRegister");
        }
      ,
        unsuccessfulRegister: function (error) {
          console.log(error.response);
          let str = "";
          for (let property in error.response) {
            if (error.response.hasOwnProperty(property) && property != "code") {
              str += error.response[property] + '\n';
            }
          }
          this.errorMessage = str;
        }
      }

    }
</script>

<style scoped>
  .container {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    height: 100%;
  }

  label {
    display: block;
  }

  .card {
    border-width: 2px;
    padding-right: 0;
    padding-left: 0;
  }
</style>
