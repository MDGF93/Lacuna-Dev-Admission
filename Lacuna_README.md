# Lacuna Dev Admission

---------------------------------------

## 1. Intro

Lacuna Software just started a fictitious branch: **Lacuna Genetics**

In order to improve our research and computing power, we decided to distribute some computing operations.

Your job is to follow the documentation below and create a **C# .NET** program which communicates with our APIs and help
us complete DNA operations.

Once you enroll to the test, you have 7 days to complete it.
After completed, send us your **final project** in a ZIP file and your **Resume (Curriculum)**
to ``admissions@lacunasoftware.com``.

If you have any questions during the test, please contact us at same email address.

**Note**: This is not a race! Make sure you show your coding skills, knowledge on modularization, data serialization and
code reuse. Have fun!

## 2. Enroll

---------------------------------------

You will need to handle top secret information, so the first step is to create a user in the system and request an
AccessToken for communication with the authenticated APIs.

Base address: ``https://gene.lacuna.cc/``

### 2.1 Create user

````
'[POST] /api/users/create'
Request body
{
  // Allowed a-z, A-Z and 0-9 chars only
  // Min size 4 chars and max size 32 chars
  username: string,

  // Your email address so we are able to contact you
  email: string,

  // Min size 8 chars
  password: string //
}

Response
{
  code: string, // ['Success', 'Error']
  message?: string
}

````

### 2.2 Request AccessToken

````
'[POST] /api/users/login'
Request body
{
  username: string,
  password: string
}

Response
{
  accessToken?: string,
  code: string,// ['Success', 'Error']
  message?: string
}

````

If everything is OK, you will receive a ``Success`` response code and an ``AccessToken`` string. The access token is
meant to be used in the ``Authorization`` Header parameter as an OAuth bearer token scheme.

The access token is valid for 2 minutes, if expired the authenticated APIs will return an ``Unauthorized`` response code
with message: “Bad token: token is expired”, you will need to request a new one.

## 3. Quick Bio review

---------------------------------------

The **DNA** is a structure composed of a double-stranded helix.
The two strands are connected by hydrogen bonds and each end of the bond has a nucleobase. The DNA possible nucleobases
are **A**denine, **C**ytosine, **G**uanine and **T**hymine, in a way that **A** always pairs with **T** and **C** always
pairs with **G**

<img src="https://i.imgur.com/XCVcvsV.png">

### 3.1 DNA strands

For this **fictional experiment** we consider the main strand as the template strand, i.e. the strand which is used to
transcript RNA. Also for this experiment, the main strand segments presented will always begin with the nucleobases
sequence: **C-A-T**, so it is a simple task to differentiate the template strand from the complementary strand or to
compute one from the other just inverting the nucleobases pairs.

<img src="https://i.imgur.com/yssO3L2.png">

## 4. DNA Encoding

---------------------------------------

You shall expect DNA strand segments encoded in both binary and string formats:

### 4.1 Binary format

Is the short format used for better data transmission and storage performance. In this format the nucleobases are
encoded in 2 bits arrays:

````
A: 0b00      C: 0b01
T: 0b11      G: 0b10
````

### 4.2 String format

Used for better human understanding. In this format the nucleobases are encoded as its char:
``CATCGTCAGGACTCAGTCCATCTTAACTACTAAACTC...``

### 4.3 Encoding example

Encoding from String to Binary format example:

````
String:   "CATCGTCAGGAC"
Bits:     0b010011011011010010100001
Byte[]:   [0x4D, 0xB4, 0xA1] // notice the bits to byte conversion is Big-Endian
Base64:   "TbSh"
````

## 5. Operations

### 5.1 Request a job

``Base address: https://gene.lacuna.cc/``

````
'[GET] /api/dna/jobs'
Header
    Authorization = 'Bearer <AccessToken>' // <AccessToken> aquired on 2.2

Response
{
    job?: {
        // Job id
        id: string,
    
        // Operation types ['DecodeStrand', 'EncodeStrand', 'CheckGene']
        type: string,
        
        // Strand in String format. Non-null when operation type 'EncodeStrand'
        strand?: string,
    
        // Strand in the Binary format Base64 encoded. Non-null when operation types 'DecodeStrand' and 'CheckGene'
        strandEncoded?: string,
    
        // A gene segment in the Binary format Base64 encoded. Non-null when operation type 'CheckGene'
        geneEncoded?: string
    },
    code: string, // ['Success', 'Error', 'Unauthorized']
    message?: string
}
````

If everything is OK you will receive a job object with the job ``id``,
the operation ``type`` and operation parameters which you are able to solve as follows.

### 5.2. Decode strand operation

If you receive a ``DecodeStrand`` operation, the job is to take the ``strandEncoded`` parameter, which is a Base64
string of the strand in Binary format, and decode it to the String format according to session **4**.

For this operation you shall send the response to:

````
'[POST] /api/dna/jobs/{id}/decode'
URL Parameters
  id // The Job id

Header
  Authorization = 'Bearer <AccessToken>' // <AccessToken> aquired on 2.2

Request body
{
  // Decoded strand in String format
  strand: string,
}

Response
{
  code: string, // ['Success', 'Error', 'Fail', 'Unauthorized']
  message?: string
}
````

### 5.3 Encode strand operation

If you receive a ``EncodeStrand`` operation, the job is to take the ``strand`` parameter, which is the strand in String
format, and encode it to the Binary format according to session **4**.

For this operation you shall send the response to:

````
'[POST] /api/dna/jobs/{id}/encode'
URL Parameters
  id // The Job id

Header
  Authorization = 'Bearer <AccessToken>' // <AccessToken> aquired on 2.2

Request body
{
  // Encoded strand in Binary format Base64
  strandEncoded: string,
}

Response
{
  code: string, // ['Success', 'Error', 'Fail', 'Unauthorized']
  message?: string
}
````

### 5.4. Check gene operation

If you receive a 'CheckGene' operation, the job is to tell whether or not a particular gene is activated in the
retrieved DNA strand. Both gene and DNA strands are retrieved in Binary formats.
For this experiment, a gene is considered activated if more than 50% of its content is present in the DNA template
strand. Ex:

Gene:
TACCGCTTCA<mark>TAAACCGCTAGACTGCATGATCG</mark>GGT

DNA template strand:
CATCTCAGTCCTACTAAACTCGCGAAGCTCATACTAGCTAC<mark>TAAACCGCTAGACTGCATGATCG</mark>CATAGCTAGCTACGCT

In the example above more than 50% of the gene (~63% of the gene) is present on the template strand, so in this case the
gene is considered **activated**.

**REMARK**: Please notice that the gene comparison shall be applied over the DNA **template strand**. So you need to
check according to session **3.1** if the retrieved strand is the template or the complementary one and compute each
other if necessary before searching for the gene segments presence.

For this operation you shall send the response to:

````
'[POST] /api/dna/jobs/{id}/gene'
URL Parameters
  id // The Job id

Header
  Authorization = 'Bearer <AccessToken>' // <AccessToken> aquired on 2.2

Request body
{
  // Whether or not the gene is activated in the template strand
  isActivated: boolean,
}

Response
{
  code: string, // ['Success', 'Error', 'Fail', 'Unauthorized']
  message?: string
}
````

## 6. Final comments

---------------------------------------

> There is no knowledge that is not power

Have a fun test!!