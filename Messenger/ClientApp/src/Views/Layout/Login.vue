<template>
    <div class="container">
        <div class="card border-danger mb-3 rounded col-6">
            <div class="card-header text-center">Login</div>
            <div class="card-body">
                <form novalidate @submit.prevent="login">
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
                            <button class="w-50 btn btn-danger" type="submit">Login</button>
                        </div>
                    </div>
                </form>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: "Login",
        data: function () {
            return {
                email: null,
                password: null,
                errorMessage: null,
            }
        },
        computed: {
            isValidPassword: function () {
                return (!!this.password) && this.password.length > 7;
            },
            isValidEmail: function () {
                const re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
                return (!!this.email) && re.test(this.email);
            }
        },
        methods: {
            login: function (event) {
                let formData = new FormData(event.target);
                this.$store.dispatch("account/Login",formData,{root:true})
                .then(()=>this.successfulLogin())
                .catch((error)=>this.unsuccessfulLogin(error));
            },
            successfulLogin: function () {
                this.errorMessage = null;
                this.$router.push("/messenger");
            },
            unsuccessfulLogin: function (data) {
                let str = "";
                for (let property in data.response) {
                    if (data.response.hasOwnProperty(property) && property != "code") {
                        str += data.reponse[property] +'\n';
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

    .col-6{
        flex:none;
    }
</style>
